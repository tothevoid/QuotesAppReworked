import { ApplicationPaths, ApplicationName } from './ApiAuthorizationConstants';

export class AuthorizeService {
    _callbacks = [];
    _nextSubscriptionId = 0;
    _token = null;
    _isAuthenticated = false;

    async isAuthenticated() {
        const token = await this.getIdentity();
        return !!token;
    }

    async getIdentity() {
        return localStorage.getItem("token");
    }

    async getAccessToken() {
        return this.getIdentity();
    }

    async getUser() {
        //parse jwt and get user
        return "test";
    } 

    async signIn(email, password) {
        const response = await fetch("api/identity/gettoken", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({email: email, password: password})
        }).then(response => response.json());
        localStorage.setItem("token", response.token);
        this.updateState(response.token);
    }

    async signOut() {
        localStorage.removeItem("token");
        this.updateState(undefined);
        return true;
    }

    async getAuthHeaders() {
        const token = await authService.getAccessToken();
        return !token ? {} : { 'Authorization': `Bearer ${token}` }
    }

    updateState(token) {
        this._token = token;
        this._isAuthenticated = !!this._token;
        this.notifySubscribers();
    }

    subscribe(callback) {
        this._callbacks.push({ callback, subscription: this._nextSubscriptionId++ });
        return this._nextSubscriptionId - 1;
    }

    unsubscribe(subscriptionId) {
        const subscriptionIndex = this._callbacks
            .map((element, index) => element.subscription === subscriptionId ? { found: true, index } : { found: false })
            .filter(element => element.found === true);
        if (subscriptionIndex.length !== 1) {
            throw new Error(`Found an invalid number of subscriptions ${subscriptionIndex.length}`);
        }

        this._callbacks.splice(subscriptionIndex[0].index, 1);
    }

    notifySubscribers() {
        for (let i = 0; i < this._callbacks.length; i++) {
            const callback = this._callbacks[i].callback;
            callback();
        }
    }
    static get instance() { return authService }
}

const authService = new AuthorizeService();

export default authService;

export const AuthenticationResultStatus = {
    Redirect: 'redirect',
    Success: 'success',
    Fail: 'fail'
};
