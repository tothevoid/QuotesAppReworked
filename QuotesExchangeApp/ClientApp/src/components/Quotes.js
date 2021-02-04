import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class Quotes extends Component {
    static displayName = Quotes.name;

    constructor(props) {
        super(props);
        this.state = { quotes: [], loading: true };
    }

    componentDidMount() {
        this.getQuotes();
    }

    static renderedQuotesTable(quotes) {
        if (!quotes)
            return <p>Nothing</p>
        else {
            return (
                <div className="div-custom">
                <table className="table custom-table">
                    <thead>
                        <tr><th>Company</th><th>Ticker</th><th>Price</th><th>Date</th></tr>
                    </thead>
                    <tbody>
                        {
                            quotes.map((quotes, ix) => 
                                <tr key={ix}>
                                    <td>{quotes.companyName}</td>
                                    <td>{quotes.companyTicker}</td>
                                    <td>{quotes.quotePrice}$</td>
                                    <td>{quotes.quoteDate}</td>
                                </tr>
                            )
                        }
                    </tbody>
                </table>
                </div>
            );
        }   
    }

    render () {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Quotes.renderedQuotesTable(this.state.quotes);

        return (
            <div>
                <div>
                    <h2>Quotes list</h2>
                    {contents}
                </div>
            </div>
        );
    }

    async getQuotes() {
        const token = await authService.getAccessToken();
        // {
        //     headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        // }
        const response = await fetch('api/quotes');
        const data = await response.json();
            this.setState({ quotes: data, loading: false });
        }
    }
