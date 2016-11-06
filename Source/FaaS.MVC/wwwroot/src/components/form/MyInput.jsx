import React, { Component } from "react";


class MyInputComponent extends Component {

    constructor(props) {
        super(props);
        this.onChange = this.onChange.bind(this);
    }

    componentDidMount(){
        this.setState({value: ""});
    }

    onChange(e) {
        this.setState({value: e.target.value});
    }

    render() {
        return (
            <div className="form-group">
                <label htmlFor={this.props.id} className="col-md-5 control-label">
                    {this.props.label}
                </label>
                <input type="text" 
                    id={this.props.id}
                    onChange={this.onChange}
                    className="form-control"/>
            </div>
        );
    }
}

export default MyInputComponent;