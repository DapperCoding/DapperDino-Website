<template>


    <div class="card mb50 wow bounceInDown animated" data-wow-duration="750ms">
        <div class="card-header">
            <h4 class="my-0 font-weight-normal">{{ product.name }}</h4>
        </div>
        <div class="card-body">
            <h1 class="card-title pricing-card-title">${{product.price}}<small class="text-muted">.99</small></h1>
            <p>{{ product.shortDescription }}</p>
            <a href="/Products/RuneScapeBot" class="btn btn-lg btn-block btn-primary">Request contact</a>
        </div>
    </div>
</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { log } from 'util';
    //import axios from 'axios'
    //import { Card, createToken } from 'vue-stripe-elements-plus'

    @Component({
        name: 'Product',
        computed: {
        },
        components: { },
        methods: {
           
        },
        props: ['product']
    })
    export default class Product extends Vue {
        

        
    }
</script>

<style scoped>
    
</style>
