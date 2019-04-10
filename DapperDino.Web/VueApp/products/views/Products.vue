<template>


    <div class="container">

        <div class="row text-center" style="margin-bottom:30px;">

            <div class="sec-title text-center mb50 wow bounceInDown animated" data-wow-duration="500ms">
                <h2>Discord Bots</h2>
                <div class="devider"><i class="fa fa-heart-o fa-lg"></i></div>
                <p>By buying a discord bot, you'll receive the code. The code has comments on most lines, to help out beginners and explain how everything works.</p>
            </div>
            <div class="col-md-6">
                <div class="card mb50 wow bounceInDown animated" data-wow-duration="750ms">
                    <div class="card-header">
                        <h4 class="my-0 font-weight-normal">RS Gamble Bot</h4>
                    </div>
                    <div class="card-body">
                        <h1 class="card-title pricing-card-title">$199<small class="text-muted">.99</small></h1>
                        <p>More information coming soon, please request contact for more information.</p>
                        <a href="/Products/RuneScapeBot" class="btn btn-lg btn-block btn-primary">Request contact</a>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card mb50 wow bounceInDown animated" data-wow-duration="1250ms">
                    <div class="card-header">
                        <h4 class="my-0 font-weight-normal">Music bot</h4>
                    </div>
                    <div class="card-body">
                        <h1 class="card-title pricing-card-title">$4<small class="text-muted">.99</small></h1>
                        <p>Full fledged discord bot to manage your music.</p>
                        <p>More information coming soon, please request contact for more information.</p>
                        <a href="/Products/MusicBot" class="btn btn-lg btn-block btn-primary">Request contact</a>
                    </div>
                </div>
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
        },
        components: { /*Card*/ },
        methods: {
            //pay() {
            //    // createToken returns a Promise which resolves in a result object with
            //    // either a token or an error key.
            //    // See https://stripe.com/docs/api#tokens for the token object.
            //    // See https://stripe.com/docs/api#errors for the error object.
            //    // More general https://stripe.com/docs/stripe.js#stripe-create-token.
            //    createToken().then((data:any) => console.log(data.token))
            //}
        }
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
