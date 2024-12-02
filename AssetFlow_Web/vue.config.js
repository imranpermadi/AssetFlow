module.exports = {
	publicPath: process.env.NODE_ENV === 'production' ? '/' : '/',
	devServer: {
		hot: true,
		host: 'localhost'
	},

}