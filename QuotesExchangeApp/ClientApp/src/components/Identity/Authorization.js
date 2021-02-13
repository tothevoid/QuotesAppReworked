import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService';
import {  QueryParameterNames } from '../api-authorization/ApiAuthorizationConstants';

export class Authorization extends Component {

    constructor(props) {
        super(props);
        this.state = { email: "", password: ""};
    }

    handleChange = ({ target: { name, value } }) => {
        this.setState({ [name]: value})
    }

    render () {
        return  <div className="row login-container">
            <div className="col-4  shadow-lg">
                <h5>Добро пожаловать на биржу</h5>
                <form id="account" method="post">
                    <hr />
                    <div className="form-group">
                        <label>Email</label>
                        <input name="email" value={this.state.login} onChange={this.handleChange} className="form-control" />
                    </div>
                    <div className="form-group">
                        <label>Пароль</label>
                        <input name="password" type="password"  value={this.state.password} onChange={this.handleChange}  className="form-control" />
                    </div>
                    <div className="form-group">
                        <button type="submit" onClick={(event)=>{event.preventDefault(); this.login()}} className="btn btn-primary login-btn">Войти</button>
                    </div>
                </form>
            </div>
        </div>
    }
    
    async login() {
        await authService.signIn(this.state.email, this.state.password);
        const url = this.getReturnUrl(this.state);
        this.navigateToReturnUrl(url);
    }

    getReturnUrl(state) {
        const params = new URLSearchParams(window.location.search);
        const fromQuery = params.get(QueryParameterNames.ReturnUrl);
        if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.")
        }
        return (state && state.returnUrl) || fromQuery || `${window.location.origin}/`;
    }

    navigateToReturnUrl(returnUrl) {
        window.location.replace(returnUrl);
    }
}

