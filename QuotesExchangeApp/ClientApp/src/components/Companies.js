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

    static renderedCompaniesTable(companies) {
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
        : Companies.renderedCompaniesTable(this.state.companies);

    return (
      <div>
        <h2>Companies list</h2>
        {contents}
      </div>
    );
  }

  async getCompanies() {
    const response = await fetch('api/company', {
      headers: await authService.getAuthHeaders()
    });
    const data = await response.json();
      this.setState({ companies: data, loading: false });
  }
}
