<template>


    <div></div>
</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { log } from 'util';
    import axios from 'axios'

    @Component({
        name: 'Products',
        computed: {
        }
    })
    export default class Products extends Vue {
        ticketId: number = 1200;

        created() {

        }
    }
</script>

<style scoped>

</style>
