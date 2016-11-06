const path = require("path");

module.exports = {
    entry: {
        app: "./src/app.js",
        form: "./src/form.js"
    },
    output: {
        path: path.join(__dirname, "out"), // All of the output filles will be generated to out directory
        filename: "[name].out.js"
    },
    module: {
        loaders: [
            { test: /\.css$/, loader: "style!css" },
            { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' },
            {
                test: /\.jsx?$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel',
                query: {
                    presets: ['stage-1', 'es2015', 'react']
                }
            }
        ]
    },
    resolve: {
        extensions: ['', '.js', '.jsx']
    }
};