<template>
    <div>
        <div class="container-flex">
            <div class="row">
                <div class="col-xs-12">
                    <section class="section">
                        <aside class="menu">
                            <ul class="menu-list">
                                <li>
                                    <!--<router-link :to="{ name: 'ticketWrapper' }">Tickets</router-link>-->
                                </li>
                            </ul>
                        </aside>
                    </section>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">

                    <section class="section">
                        <transition name="component-fade"
                                    mode="out-in">
                            <router-view></router-view>
                        </transition>
                    </section>

                </div>
            </div>
        </div>
        <notifications group="global" />
    </div>
</template>

<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator'

    import Notifications from 'vue-notification'

    Vue.use(Notifications)
    Vue.filter('reverse', function (value) {
        // slice to make a copy of array, then reverse the copy
        return value.slice().reverse();
    });

    @Component({
        name: 'App'
    })

    export default class App extends Vue { }
</script>

<style scoped>
</style>
