<template>
	<div class="layout-footer w-full">
		<!-- <img alt="Logo" :src="darkTheme ? 'images/logo-dark.svg' : 'images/logo-white.svg'" height="20" class="mr-2" />
		by -->
		<!-- <span class="font-medium ml-2">
			Copyright &copy; {{currentYear}} <b>Wilmar Group Indonesia</b>. All rights reserved.
		</span> -->
		<div class="grid w-full">
			<div class="col-6">
				<span class="footer-text">
					Copyright &copy; {{currentYear}} <b>Wilmar Group Indonesia</b>. All rights reserved.
				</span>
				
			</div>
			<div class="col-6 text-right">
				<span class="footer-text">
					{{version}}
				</span>
				
			</div>
		</div>
	</div>
</template>

<script>
	export default {
		name: "AppFooter",
		data(){
			return {
				version: "[] v.0.0",
			}
		},
		computed: {
			darkTheme() {
				return this.$appState.theme.startsWith('saga');
			},
			currentYear(){
				return new Date().getFullYear();
			}
		},
		beforeMount(){
			var self = this;
			this.$axios.get(this.$UTIL_URL + "version")
                    .then((response) => {
                        if(response && response.data.success){
                            self.version = response.data.data;
                        }
                        
                    })
                    .catch((error) => {
                        //self.showError('Error Getting Order Type Data', error);
                    });

		}
	}
</script>

<style scoped>

</style>