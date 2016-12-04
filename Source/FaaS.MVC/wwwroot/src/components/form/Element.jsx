import React from 'react';
import ReactDOM from "react-dom";
import InputField from './InputField.jsx'

class Element extends React.Component {
    constructor(props) {
        super(props);
		
        this.state = {  id: this.props.content.id,
                        description: this.props.content.description,
                        options: JSON.parse(this.props.content.options),
                        type: this.props.content.type,
                        requiredLabel: false
                     };

    };

    setRequiredLabel(value){
        this.setState({requiredLabel: value})
        this.refs["label" + this.state.id].setVisible(value);
    };

    render() {
        return (
            <div>
                <h4>{this.state.description}
                <Asterisk visible={this.props.content.required}/> </h4>
                <RequiredLabel ref={"label" + this.state.id} visible={this.state.requiredLabel} color="red"/>
                <InputField type={this.state.type} options={this.state.options} elementId={this.state.id} valueChanged={this.props.valueChanged} />
            </div>
        );
    };
}

class Asterisk extends React.Component{
    constructor(props) {
        super(props);
        
        this.state = {
            visible: this.props.visible
        };        
    };

    setVisible(value){
        //this.setState({visible: value});
        this.updateState({visible: value});
        this.forceUpdate();
    };

    render(){
        if(!this.state.visible)
            return(
                <div/>
            );
        else{
            return(
                <font color="red">
                    *
                </font>
            )
        }
    };
}

class RequiredLabel extends React.Component{
    constructor(props) {
        super(props);
        
        this.state = {
            visible: this.props.visible
        };        
    };

    setVisible(value){
        this.setState({visible: value});
    };

    render(){
        if(!this.state.visible)
            return(
                <div>
                </div>
            );
        else{
            return(
                <div>
                    <font color="red">
                        <label >This field is required</label>
                    </font>
                </div>
            )
        }
    };

}


export default  Element;
