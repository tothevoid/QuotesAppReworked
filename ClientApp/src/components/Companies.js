import React, { Component } from 'react';
import authService  from './api-authorization/AuthorizeService'

export class Companies extends Component {
 
  constructor(props) {
    super(props);
    this.state = { companies: [], quotes: [], loading: true };
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
          {this.renderSources(this.state.quotes)}
          {this.renderedCompaniesTable(this.state.companies)}
        </div>

    return (
      <div>
        <h2>Companies list</h2>
        {contents}
      </div>
    );
  }

  renderSources(quotes){
    if (!quotes || quotes.length === 0) return <div></div>;
    return <div className="suggested-bar">
          <p>Select suggested source</p>
          <div className="suggested-sources-cotainer">
          {
            quotes.map((quote, ix)=>{
                return <div key={ix} className="suggested-source"onClick={()=>this.onSourceSelected(quote)} >
                    <p className="suggested-parameter">Source: {quote.source.name}</p>
                    <p className="suggested-parameter">Acutal price: {quote.price}$</p>
                </div>
            })
          }
          </div>
      </div>
  }asd

  async onSourceSelected(source){
    const addedCompany = await fetch("api/company", {
      method: "POST",
      headers: {"Content-Type": "application/json", ...await authService.getAuthHeaders()},
      body: JSON.stringify(source)
    }).then(response => response.json());

    if (addedCompany){
      this.setState({companies: [...this.state.companies, addedCompany],
        quotes: [], companyName: "", companyTicker: ""})
    }
  }

  renderNewCompanyForm() {
    return <div className="new-company-form">
      <h2>New company</h2>
      <label>Ticker</label>
      <input className="new-company-input" name="companyTicker" onChange={this.handleChange} type="text"></input>
      <label>Alias</label>
      <input className="new-company-input" name="companyName" onChange={this.handleChange} type="text"></input>
      <button onClick={()=>{this.onCompanyAddClick()}}>Add</button>
    </div>
  }

  async onCompanyAddClick(){
    const {companyName, companyTicker} = this.state;
    debugger;
    if (companyName !== "" && companyTicker !== "")
    {
      const response = await fetch("api/quotes/getbycompany", {
        method: "POST",
        headers: {"Content-Type": "application/json", ...await authService.getAuthHeaders()},
        body: JSON.stringify({name: companyName, ticker: companyTicker})
      }).then(response => response.json());
  
      this.setState({quotes: response.filter((element)=> element)});
    }
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
