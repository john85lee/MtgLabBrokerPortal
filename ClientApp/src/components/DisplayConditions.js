import React, { Component } from 'react';

export class DisplayConditions extends Component {
  static displayName = DisplayConditions.name;

  constructor(props) {
    super(props);
    this.state = { 
      conditions: [],
      loading: true ,
    };
  }

  componentDidMount() {
  }

    static renderByteProDBTable(conditions) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Request Date</th>
            <th>Requested By</th>
            <th>Description</th>
            <th>Condition Type</th>
          </tr>
        </thead>
        <tbody>
              {conditions.map(conditions =>
              <tr key={conditions.requestedDate}>
                  <td>{conditions.requestedDate}</td>
                  <td>{conditions.requestedBy}</td>
                  <td>{conditions.description}</td>
                  <td>{conditions.conditionTypeAndNo}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    
    let dbCont = this.state.loading
      ? <p><em>(Database Content Not Loaded)</em></p>
        : DisplayConditions.renderByteProDBTable(this.state.conditions);

    return (
      <div>
        {/* OnClick - Call Function To Fetch Server Data */}
        <button onClick={ ()=> this.getSomeDataFromServer() }>
          Get Conditions Data
        </button>
        {dbCont}

      </div>
    );
  }

  // Function To Retrieve 
  async getSomeDataFromServer() {
    const response = await fetch('conditions');
    const data = await response.json();
      this.setState({ conditions: data, loading: false });
  }

}
