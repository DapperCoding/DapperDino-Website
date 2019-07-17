<template>

    <div class="col-md-6">
        <div class="card mb50 wow bounceInDown animated" data-wow-duration="750ms">
            <div class="card-header">
                <h4 class="my-0 font-weight-normal">{{ product.name }}</h4>
            </div>
            <div class="card-body">
                <h1 class="card-title pricing-card-title">${{firstPricePart}}<small class="text-muted">.{{secondPricePart}}</small></h1>
                <p>{{ product.shortDescription }}</p>
                <a :href="'/Products/Information/' + product.id" class="btn btn-lg btn-block btn-primary">More information</a>
            </div>
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
            firstPricePart: function () {
                let productPrice = this.product.price.toString();
                let parts = productPrice.split(".");
                return parts[0];
            },
            secondPricePart: function () {
                let productPrice = this.product.price.toString();
                let parts = productPrice.split(".");
                 
                return parts.length <= 1 ? "00" : parts[1];
            }
        },
        components: {},
        methods: {

        },
        props: ['product'],
        data: function () {
            return {
                p: this.product
            }
        }
    })
    export default class Product extends Vue {

        created() {
            console.log(product)
        }

    }
</script>

<style scoped>
</style>
