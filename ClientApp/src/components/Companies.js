import React, { Component } from 'react';
import authService  from './api-authorization/AuthorizeService'

export class Companies extends Component {
 
  constructor(props) {
    super(props);
    this.state = { companies: [], loading: true };
  }

  componentDidMount() {
    this.getCompanies();
  }

  renderedCompaniesTable(companies) {
      return (
          <div className="div-custom">
              <table className="table custom-table table-company">
                  <thead>
                      <tr>
                          <th>Name</th>
                          <th>Ticker</th>
                      </tr>
                  </thead>
                  <tbody>
                      {companies.map((company, ix) =>
                          <tr key={ix}><td>{company.name}</td><td>{company.ticker}</td></tr>)
                      }
                  </tbody>
              </table>
          </div>
      );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
        : <div>
          {this.renderNewCompanyForm()}
          {this.renderedCompaniesTable(this.state.companies)}
        </div>

    return (
      <div>
        <h2>Companies list</h2>
        {contents}
      </div>
    );
  }

  renderNewCompanyForm() {
    return <div className="new-company-form">
      <h2>New company</h2>
      <label>Ticker</label>
      <input name="companyTicker" onChange={this.handleChange} type="text"></input>
      <label>Alias</label>
      <input name="companyName" onChange={this.handleChange} type="text"></input>
      <button onClick={()=>{this.onCompanyAddClick()}}>Add</button>
    </div>
  }

  onCompanyAddClick(){

  }

  handleChange = ({ target: { name, value } }) => {
    this.setState({ [name]: value})
  }

  async getCompanies() {
    const response = await fetch('api/company', {
      headers: await authService.getAuthHeaders()
    });
    const data = await response.json();
      this.setState({ companies: data, loading: false });
  }
}
