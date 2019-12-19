import React, { Component } from 'react';

export class DisplayFileData extends Component {
    static displayName = DisplayFileData.name;

  constructor(props) {
    super(props);
    this.state = { 
      fileData: [],
      loading: true ,
    };
  }

  componentDidMount() {
  }

    static renderByteProDBTable(fileData) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date Created</th>
            <th>Date Modified</th>
            <th>Borrower Names</th>
            <th>File Name</th>
          </tr>
        </thead>
        <tbody>
                {fileData.map(fileData =>
                  <tr key={fileData.dateCreated}>
                    <td>{fileData.dateCreated}</td>
                    <td>{fileData.dateModified}</td>
                    <td>{fileData.borrowerName}</td>
                    <td>{fileData.fileName}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    
    let dbCont = this.state.loading
      ? <p><em>(Database Content Not Loaded)</em></p>
        : DisplayFileData.renderByteProDBTable(this.state.fileData);

    return (
      <div>
        {/* OnClick - Call Function To Fetch Server Data */}
        <button onClick={ ()=> this.getSomeDataFromServer() }>
          Get File Data
        </button>
        {dbCont}

      </div>
    );
  }

  // Function To Retrieve 
  async getSomeDataFromServer() {
    const response = await fetch('filedata');
    const data = await response.json();
      this.setState({ fileData: data, loading: false });
  }

}
