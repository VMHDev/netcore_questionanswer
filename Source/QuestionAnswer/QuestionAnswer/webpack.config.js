/// <binding BeforeBuild='Run - Development' ProjectOpened='Watch - Development' />
"use strict";
const { VueLoaderPlugin } = require('vue-loader');
const path = require('path');
const webpack = require('webpack');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = {
    mode: 'development',
    entry: {
        index: './Views/Home/index.cshtml.js',
        layout: './Views/layout.js',
        error: './Views/Error/error.cshtml.js'
    },
    plugins: [
        new webpack.ProvidePlugin({
            '$': 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js', 'default'],
            axios: 'axios'
        }),
        new VueLoaderPlugin()
    ],

    optimization: {
        minimizer: [
            // we specify a custom UglifyJsPlugin here to get source maps in production
            new UglifyJsPlugin({
                cache: true,
                parallel: true,
                uglifyOptions: {
                    compress: false,
                    ecma: 6,
                    mangle: true
                },
                sourceMap: true
            })
        ]
    },
    output: {
        publicPath: "/dest/js/",
        path: path.join(__dirname, '/wwwroot/dest/js/'),
        filename: '[name].bundle.js'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /(node_modules)/,
                query: {
                    presets: ['es2017']
                }
            },
            {
                test: /\.css$/,
                loaders: ['style-loader', 'css-loader']
            },
            {
                test: /\.(png|jpg|gif)$/,
                use: {
                    loader: 'url-loader',
                    options: {
                        limit: 8192
                    }
                }
            },
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            }
        ]
    },
    resolve: {
        alias: {
            // VMH Note: Khai báo alias cho các folder, file project
            vue: 'vue/dist/vue.js',
            '@Views': path.resolve(__dirname, './Views'),
            '@Components': path.resolve(__dirname, './Views/Components')
        },
        extensions: ['.js', '.vue']
    }
};