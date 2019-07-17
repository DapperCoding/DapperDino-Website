<template>


    <div class="container">

        <div class="row text-center" style="margin-bottom:30px;">

            <div class="sec-title text-center mb50 wow bounceInDown animated" data-wow-duration="500ms">
                <h2>Discord Bots</h2>
                <div class="devider"><i class="fa fa-heart-o fa-lg"></i></div>
                <p>By buying a discord bot, you'll receive the code. The code has comments on most lines, to help out beginners and explain how everything works.</p>
            </div>
            <div v-for="product in products">
                <Product v-bind:product="product"></Product>
            </div>
            
        </div>

        <div class="row">
            <div class="col-xs-12">
                <small>* Includes downloads of updated versions of the same project, does not include help to configure if you've changed the previous version.</small>
            </div>

        </div>
    </div>
</template>

<script lang="ts">

    import { Vue, Component } from 'vue-property-decorator'
    import { mapGetters } from 'vuex'

    import { Notification } from "@/common/services/notificationService";
    import { log } from 'util';
    import axios from 'axios'
    //import { Card, createToken } from 'vue-stripe-elements-plus'

    @Component({
        name: 'Products',

        computed: {
            ...mapGetters(["products"])
        },
        components: { /*Card*/ },
        methods: {
           
        },
        
    })
    export default class Products extends Vue {

        created() {
            // Get products
            this.$store.dispatch('getProducts')
                .then(result => {

                    //Notification.success(this, 'Setup completed')

                }).catch(error => {

                    Notification.error(this, 'Wasn\'t able to fetch the current user!')

                })

        }

    }
</script>

<style scoped>
    /*.stripe-card {
        width: 300px;
        border: 1px solid grey;
    }

    .stripe-card.complete {
        border-color: green;
    }*/
</style>
