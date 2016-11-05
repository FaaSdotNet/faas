const path = require("path");

module.exports = {
    entry: {
        login:  "./src/login.js",
        dashboard: "./src/dashboard.js",
        form: "./src/form.js"
    },
    output: {
        path: path.join(__dirname, "out"), // All of the output filles will be generated to out directory
        filename: "[name].out.js"
    },
    module: {
        loaders: [
            { test: /\.css$/, loader: "style!css" },
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
        extensions: ['', '.js', '.jsx'],
    },
};