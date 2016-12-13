import React from 'react';
import {Checkbox, CheckboxGroup} from 'react-checkbox-group';
import DatePicker from 'react-datepicker'

import 'react-datepicker/dist/react-datepicker.css';

export default class InputField extends React.Component {
    constructor(props) {
        super(props);
        
        this.state = {
            elementId: this.props.elementId,
            type: this.props.type,
            value: this.props.value,
            options: this.props.options,
            checked: []
        }
    };
    
    onValueChanged (e) {
        this.setState({
            value: e.target.value
        });
      this.props.valueChanged(this.state.elementId, e.target.value);
    };

    
    onDateChanged(date) {
        this.setState({
            value: date
        });
      this.props.valueChanged(this.state.elementId, date);
    };

    onCheckboxChanged (newChecked) {
        this.setState({
        checked: newChecked
        });
        this.setState({value: newChecked});
        
      this.props.valueChanged(this.state.elementId, this.arrayToString(newChecked));
    }

    
    arrayToString(array){
        let resultString = "";
        for(let key in array){
				resultString += array[key] + " "; // TODO ???????
        }

        return resultString; 
    }


    render() {
		let arrayOptions = [];
        switch(this.state.type){
            case 0:
                arrayOptions = [];
                for(let key in this.props.options){
                    arrayOptions.push(<div key={this.props.elementId + key}><label key={this.props.elementId + key + 'label'}><Checkbox key={this.props.elementId + key + 'chckbx'} value={key} /> {this.props.options[key]} </label></div>);
                }
                return (
                    <div className="form-group">
                        <CheckboxGroup key={this.props.elementId + 'checkbox'} name={this.state.elementId} value={this.state.checked}
                            onChange={this.onCheckboxChanged.bind(this)}>
 
                        {arrayOptions}
                        </CheckboxGroup>
                    </div>
                );
            case 1:
                return(
                    <div className="form-group" key={this.props.elementId}>
                        <DatePicker inline placeholderText="Click to select a date" selected={this.state.value} onChange={this.onDateChanged.bind(this)} />
                    </div>
                );
            case 2:
                arrayOptions = [];
                for(let key in this.props.options){
                    if(this.props.options.hasOwnProperty(key)) {
						arrayOptions.push(this.props.options[key]);
					}
                }
                let radioOptions = arrayOptions.map(function(result){
                    return (
                        <div className="form-group" key={this.props.elementId + result}>
                            <input className="form-check-input" type="radio" name={this.state.elementId}
                                           value={result} checked={this.state.value === result} 
                                        onChange={this.onValueChanged.bind(this)} />
                            {result}
                        </div>
                    );
                }, this);
                return (
                    <div key={this.props.elementId}>
                        {radioOptions}
                    </div>
                );
            case 3:
                return(
                    <div className="form-group" key={this.props.elementId}>
                        <label>{this.state.value}</label>
                        <input className="form-control" type="range" name={this.state.elementId} onChange={this.onValueChanged.bind(this)} min={this.state.options.min} max={this.state.options.max} />
                    </div>
                );
            case 4:
                return(
                    <div className="form-group" key={this.props.elementId}>
                        <input className="form-control" type="text" name={this.state.elementId} onChange={this.onValueChanged.bind(this)}/>
                    </div>
                );   
            case 5:
                return(
                    <div className="form-group" key={this.props.elementId}>
                        <textarea className="form-control"  name={this.state.elementId} onChange={this.onValueChanged.bind(this)}/>
                    </div>
                );              
        }
    };
}
