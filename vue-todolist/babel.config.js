// module.exports = {
//   presets: [
//     '@vue/cli-plugin-babel/preset'
//   ]
// }

module.exports = {
  presets: [
    [
      '@babel/preset-env',
      {
        // Specify which browsers you want to support
        targets: {
          browsers: [
            "last 2 versions", // Support last 2 versions of all browsers
            "ie >= 11",        // Example: add support for IE 11
            "safari >= 10"     // Example: add support for older Safari versions
          ]
        },
        // Automatically handle polyfills based on usage and targets
        useBuiltIns: "usage",
        corejs: 3, // Specify the version of core-js to be used for polyfills
      },
    ]
  ]
};
