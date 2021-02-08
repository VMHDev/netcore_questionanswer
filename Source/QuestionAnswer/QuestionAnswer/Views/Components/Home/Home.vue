<template src="./Home.vue.html">
</template>
<style scoped src="./Home.vue.css"></style>

<script>
    export default {
        name: "Home",
        data() {
            return {

            }
        },
        mounted: function () {
            this.init();
        },
        methods: {
            // Load data init
            async init() {
                await this.apiGetListTag();
            },

            async apiGetListTag() {
                await axios.get('/api/getlisttag')
                    .then((res) => {
                        console.log('res', res);
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
        }
    }
</script>