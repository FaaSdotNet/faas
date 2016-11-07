import React, { Component } from "react";


class MySubmitComponent extends Component {

    constructor(props) {
        super(props);   
        this.onClick = this.onClick.bind(this);     
    }

    onClick(event)
    {
        this.props.onClick();
    }

    render() {
        return (
            <div className="form-group">
                <div className="col-md-offset-3 col-md-10">
                    <input type="button" 
                        id={this.props.id}
                        onClick={this.onClick}
                        value={this.props.value} 
                        className="btn btn-default"/>
                </div>
            </div>
        );
    }
}

export default MySubmitComponent;