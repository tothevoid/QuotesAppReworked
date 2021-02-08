import React, { Component } from 'react';

export class Login extends Component {

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
                        <input name="password" value={this.state.password} onChange={this.handleChange}  className="form-control" />
                    </div>
                    <div className="form-group">
                        <button type="submit" onClick={(event)=>{event.preventDefault(); this.login()}} className="btn btn-primary login-btn">Войти</button>
                    </div>
                </form>
            </div>
        </div>
    
    }
    
    async login() {
        const {email, password} = this.state;
        debugger;
        const response = await fetch('api/identity/login', {
            method: "post",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({email: email, password: password})
        });
        const data = await response.json();
        this.setState({ quotes: data, loading: false });
    }

}

