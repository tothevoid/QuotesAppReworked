import React, { Component } from 'react';
import {CanvasJSChart} from 'canvasjs-react-charts'



export class Chart extends Component {
    static displayName = Chart.name;

    constructor(props) {
        super(props);
        this.state = { quotes: [], companies: [], 
            loading: true, currentCompany: null,
            currentButton: Chart.getButtons()[1] };
    }

    static getButtons(){
        return [
            { caption: "5 минут", value: 5 },
            { caption: "1 час", value: 60},
            { caption: "4 часа", value: 240 },
            { caption: "1 день", value: 1440 },
            { caption: "1 неделя", value: 10080 },
            { caption: "1 месяц", value: 43200 },
            { caption: "1 год", value: 525600 },
            { caption: "Макс. ", value: 10000000 },
        ]
    }

    componentDidMount() {
        this.getCompanies();
    }

    onCompanySelected = (company) =>{
        if (company !== this.state.currentCompany){
            this.setState({ currentCompany: company});
            this.getQuotes(company, this.state.currentButton)
        }
    }

    onSpanSelected = (button) => {
        debugger;
        if (button !== this.state.currentButton){
            this.setState({ currentButton: button});
            this.getQuotes(this.state.currentCompany, button);
        }
    }

    static renderChart(quotes) {
        if (!quotes)
            return <p>Nothing</p>
        else {
            const data = quotes.map(function (quote) {
                return { x: new Date(quote.quoteDate), y: quote.quotePrice }
            });
            const options = {
                theme: "light2",
                animationEnabled: true,
                zoomEnabled: true,
                title: {
                    text: "График цен"
                },
                data: [{
                    type: "area",
                    dataPoints: data,
                    color: "#f0b90b"
                }]
            };
            return (<CanvasJSChart options = {options}/>)
        }   
    }

    renderCompanies = (companies, currentCompany) => {

        const formatDate = (date) => date.getDate() + "/" + date.getMonth() + "/" + date.getFullYear() 
            + " " + date.getHours() + ":" + date.getMinutes();

        return (
            <div className="col-sm-4 div-custom shadow">
                <table className="table custom-table">
                    <thead>
                        <tr><th>Тикер</th><th>Цена</th><th>Дата</th></tr>
                    </thead>
                    <tbody>
                        {companies.map((company, ix) => {
                            const decorationClass = currentCompany === company ?
                                "company-selected company-item": "company-item"
                            return <tr onClick={()=>this.onCompanySelected(company)} className={decorationClass} key={ix}>
                                <td>
                                    <div 
                                        className="form-ticker-right" >
                                        <div>{company.companyTicker}</div>
                                    </div>
                                </td>
                                <td>{company.quotePrice}$</td>
                                <td>{formatDate(new Date(company.quoteDate))}</td>
                            </tr>}
                        )}
                    </tbody>
                </table>
            </div>)
    }

    renderButtons = (buttons, chart) => {
        return (<div className="col-sm-8 div-custom shadow">
            <div>
                {chart}
                <div className="timespan-container">
                    {buttons.map((button, ix)=>{
                        const decorationClass = button.caption === this.state.currentButton.caption ?
                            "timespan-selected timespan-btn": "timespan-btn"
                        return  <div onClick={()=>this.onSpanSelected(button)} key={ix} className={decorationClass}>{button.caption}</div>
                        }
                    )}
                </div>
            </div>
        </div>)
    }

    render () {
        let chart = this.state.loading
            ? <p><em>Loading...</em></p>
            : Chart.renderChart(this.state.quotes);
        const {currentCompany} = this.state;
        const title = (currentCompany) ?
            <h3>Акции {currentCompany.companyName} ({currentCompany.companyTicker})</h3> :
            <h3>Chart</h3>

        return (
            <div>
                <div>
                    {title}
                    <div className="row">
                        {this.renderButtons(Chart.getButtons(), chart)}
                        {this.renderCompanies(this.state.companies, this.state.currentCompany)}
                    </div>
                </div>
            </div>
        );
    }
    
    async getCompanies() {
        const response = await fetch('api/chart');
        const data = await response.json();
        if (data && data.length !== 0){
            const currentCompany = data[0];
            this.setState({ companies: data, currentCompany: currentCompany});
            this.getQuotes(currentCompany, this.state.currentButton)
        }
    }

    async getQuotes(company, span) {
        debugger;
        const response = await fetch('api/chart', {
            method: "post",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({companyId: company.companyId, mins: span.value})
        });
        const data = await response.json();
        this.setState({ quotes: data, loading: false });
    }
}