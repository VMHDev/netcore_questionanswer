<template src="./MainSidebarRight.vue.html">
</template>
<style scoped src="./MainSidebarRight.vue.css"></style>

<script>
    export default {
        name: "MainSidebarRight",
        data() {
            return {
                dataLstTag: [],
                dataLstTagRight: [],
                dataLstTagLeft: []
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
            // API Get list tag
            async apiGetListTag() {
                await axios.get('/api/getlisttag')
                    .then((res) => {
                        console.log('res', res);
                        this.dataLstTag = res.data;
                        for (let i = 0; i < this.dataLstTag.length; i++) {
                            if (i % 2 == 0) {
                                this.dataLstTagLeft.push(this.dataLstTag[i]);
                            } else {
                                this.dataLstTagRight.push(this.dataLstTag[i]);
                            }
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
        }
    }
</script>