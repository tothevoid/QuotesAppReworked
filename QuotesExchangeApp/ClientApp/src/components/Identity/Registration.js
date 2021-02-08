import React, { Component } from 'react';

export class Registration extends Component {

    constructor(props) {
        super(props);
        this.state = { email: "", password: "", passwordConfirm: ""};
    }

    handleChange = ({ target: { name, value } }) => {
        this.setState({ [name]: value})
    }

    render () {
       return <div class="row login-container">
            <div class="col-4 shadow-lg">
                <form>
                    <h5>Создайте новый аккаунт</h5>
                    <hr />
                    <div className="form-group">
                        <label>Email</label>
                        <input name="email" value={this.state.login} onChange={this.handleChange} className="form-control" />
                    </div>
                    <div className="form-group">
                        <label>Password</label>
                        <input name="password" value={this.state.password} onChange={this.handleChange}  className="form-control" />
                    </div>
                    <div className="form-group">
                        <button type="submit" onClick={(event)=>{event.preventDefault(); this.singIn()}} className="btn btn-primary login-btn">Войти</button>
                    </div>
                </form>
            </div>
        </div>
    }
    
    async singIn() {
        const {email, password} = this.state;
        debugger;
        const response = await fetch('api/identity/register', {
            method: "post",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({email: email, password: password})
        });
        const data = await response.json();
        this.setState({ quotes: data, loading: false });
    }

}

