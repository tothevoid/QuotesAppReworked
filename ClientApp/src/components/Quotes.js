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
        const formatDate = (date) => date.getDate() + "/" + date.getMonth() + "/" + date.getFullYear() 
            + " " + date.getHours() + ":" + date.getMinutes();

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
                                    <td>{quotes.name}</td>
                                    <td>{quotes.ticker}</td>
                                    <td>{quotes.lastQuoteValue}$</td>
                                    <td>{formatDate(new Date(quotes.lastQuoteDate))}</td>
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
        const response = await fetch('api/quotes/get', {
            headers: await authService.getAuthHeaders()
        });
        const data = await response.json();
            this.setState({ quotes: data, loading: false });
        }
    }
