const CopyWebpackPlugin = require("copy-webpack-plugin");
const path = require("path");

module.exports = {
  mode: "production",
  entry: {}, // 強制無 entry
  output: {
    path: path.resolve(__dirname, "wwwroot"),
  },
  plugins: [
    new CopyWebpackPlugin({
      patterns: [
        {
          from: "node_modules/fomantic-ui/dist/semantic.min.js",
          to: "dist/js/semantic.min.js",
        },
        {
          from: "node_modules/fomantic-ui/dist/components/list.min.css",
          to: "dist/css/list.min.css",
        },
        {
          from: "node_modules/fomantic-ui/dist/components/dropdown.min.css",
          to: "dist/css/dropdown.min.css",
        },
        {
          from: "node_modules/fomantic-ui/dist/components/search.min.css",
          to: "dist/css/search.min.css",
        },
        {
          from: "node_modules/fomantic-ui/dist/components/transition.min.css",
          to: "dist/css/transition.min.css",
        },
        {
          from: "node_modules/simplemde/dist/simplemde.min.js",
          to: "dist/js/simplemde.min.js",
        },
        {
          from: "node_modules/bootstrap/dist/css/bootstrap.min.css",
          to: "dist/css/bootstrap.min.css",
        },
        {
          from: "node_modules/bootstrap/dist/js/bootstrap.min.js",
          to: "dist/js/bootstrap.min.js",
        },
        {
          from: "node_modules/jquery/dist/jquery.min.js",
          to: "dist/js/jquery.min.js",
        },
        {
          from: "node_modules/jquery-serializejson/jquery.serializejson.min.js",
          to: "dist/js/jquery.serializejson.min.js",
        },
        {
          from: "node_modules/highlight.js/lib/highlight.js",
          to: "dist/js/highlight.js",
        },
        {
          from: "node_modules/highlight.js/styles/atom-one-light.css",
          to: "dist/css/atom-one-light.css",
        },
        {
          from: "node_modules/github-markdown-css/github-markdown.css",
          to: "dist/css/github-markdown.css",
        },
        {
          from: "node_modules/highlightjs-line-numbers.js/dist/highlightjs-line-numbers.min.js",
          to: "dist/js/highlightjs-line-numbers.min.js",
        },
        {
          from: "node_modules/simplemde/dist/simplemde.min.css",
          to: "dist/css/simplemde.min.css",
        },
        { from: "node_modules/anchor-js/anchor.js", to: "dist/js/anchor.js" },
        // 依需求加入其他前端套件
      ],
    }),
  ],
};
