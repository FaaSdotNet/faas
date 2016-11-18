import React from 'react';
import {Checkbox, CheckboxGroup} from 'react-checkbox-group';

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

    onCheckboxChanged (newChecked) {
        this.setState({
        checked: newChecked
        });
        this.setState({value: newChecked});
        
      this.props.valueChanged(this.state.elementId, this.arrayToString(newChecked));
    }

    
    arrayToString(array){
        var resultString = "";
        for(var key in array){
            resultString += array[key] + " ";
        }

        return resultString; 
    }


    render() {
        switch(this.state.type){
            case 0:
                var arrayOptions = [];
                for(var key in this.props.options){
                    arrayOptions.push(<div key={key}><label key={key}><Checkbox key={key} value={key} /> {this.props.options[key]} </label></div>);
                }
                return (
                    <CheckboxGroup key={0} name={this.state.elementId} value={this.state.checked}
                        onChange={this.onCheckboxChanged.bind(this)}>
 
                    {arrayOptions}
                    </CheckboxGroup>
                );
            case 1:
                return(
                    <div>
                        <input type="date" name={this.state.elementId} onChange={this.onValueChanged.bind(this)}/>
                    </div>
                );
            case 2:
                var arrayOptions = [];
                for(var key in this.props.options){
                arrayOptions.push(this.props.options[key]);
                }
                var radioOptions = arrayOptions.map(function(result){
                return (
                    <div><input type="radio" name={this.state.elementId}
                                       value={result} 
                                       checked={this.state.value === result} 
                                    onChange={this.onValueChanged.bind(this)} />
                                {result}
                            </div>
                );
                }, this);
                return (
                    <div>
                    {radioOptions}
                    </div>
                );
            case 3:
                return(
                    <div>
                        <label>{this.state.value}</label>
                        <input type="range" name={this.state.elementId} onChange={this.onValueChanged.bind(this)} min="0" max="10" />
                    </div>
                );
            case 4:
                return(
                    <div>
                    <input type="text" name={this.state.elementId} onChange={this.onValueChanged.bind(this)}/>
                    </div>
                );                    
        }
    };
}

