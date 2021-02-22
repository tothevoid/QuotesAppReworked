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
        const token = localStorage.getItem("token");
        if (token){
            const jwt = this.parseJwt(token);
            if (jwt.exp && jwt.exp * 1000 <= Date.now()){
                localStorage.setItem("token", "");
                return "";
            }
            return token;
        }
        return "";
    }


    async getAccessToken() {
        return this.getIdentity();
    }

    async getUser() {
        const token = localStorage.getItem("token");
        if (token){
            const userNameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            const jwt = await this.parseJwt(token);
            return (jwt && jwt[userNameClaim]) ? jwt[userNameClaim] : "";
        }
        return "";
    } 

    async signIn(email, password) {
        await this.requestToken(email, password, "gettoken");
    }

    async signUp(email, password) {
        await this.requestToken(email, password, "signup");
    }

    async requestToken(email, password, methodName){
        const response = await fetch("api/identity/" + methodName, {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({email: email, password: password})
        }).then(response => response.text());
        localStorage.setItem("token", response);
        this.updateState(response);
    }

    parseJwt (token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    
        return JSON.parse(jsonPayload);
    };

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
