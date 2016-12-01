import React, {Component} from "react";

export class FAQ extends Component {
	render()
	{
		return (
			<div className="row">
				<h1> FAQ </h1>
				<div className="row">
					<p>
						<strong> Why we are doing this? </strong>
					</p>
					<p>
						Because we can!
					</p>
				</div>
				<div className="row">
					<p>
						<strong> What is this project about? </strong>
					</p>
					<p>
						Javascript!
					</p>
				</div>

				<div className="row">
					<p>
						<strong> Can this be used in production? </strong>
					</p>
					<p>
						NOOOOOOOOOOOO!
					</p>
				</div>
			</div>
		);
	}
}

export default FAQ;

