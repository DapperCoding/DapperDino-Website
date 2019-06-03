<template>
    <div>
        <div class="">
            <nav class="navbar is-fixed-top is-dark" role="navigation" aria-label="dropdown navigation">
                <div class="navbar-item has-dropdown">
                    <a class="navbar-link" v-on:click="toggleClass">
                        DapperDino
                    </a>

                    <div class="navbar-dropdown is-dark">
                        <a class="navbar-item" href="/">
                            Home
                        </a>
                        <hr class="navbar-divider">
                        <a class="navbar-item" href="/Products">
                            Products
                        </a>
                        <a class="navbar-item"  href="/Hosting">
                            Hosting
                        </a>
                        <a class="navbar-item"  href="/Suggestion">
                            Suggestion
                        </a>
                        <a class="navbar-item" href="/FAQ">
                            Faq
                        </a>
                        <a class="navbar-item" href="/Apply">
                            Apply
                        </a>
                        <a class="navbar-item" href="/Contact">
                            Contact
                        </a>
                        <!--<hr class="navbar-divider">-->

                        <!--<a class="navbar-item"  href="/">
                            Client Dashboard
                        </a>
                        <a class="navbar-item">
                            My Tickets
                        </a>
                        <a class="navbar-item">
                            My Suggestions
                        </a>
                        <a class="navbar-item">
                            Manage Account
                        </a>
                        <hr class="navbar-divider">

                        <a class="navbar-item">
                            H2H Dashboard
                        </a>
                        <a class="navbar-item">
                            Tickets
                        </a>
                        <a class="navbar-item">
                            Suggestions
                        </a>
                        <a class="navbar-item">
                            Faqs
                        </a>
                        <hr class="navbar-divider" />
                        <a class="navbar-item">
                            Admin Dashboard
                        </a>
                        <a class="navbar-item">
                            Applications
                        </a>
                        <a class="navbar-item">
                            Hosting
                        </a>
                        <a class="navbar-item">
                            Products
                        </a>-->
                    </div>
                </div>

                <div class="navbar-item">
                    <router-link :to="{ name: 'ticketWrapper' }">Tickets</router-link>
                </div>
                <div class="navbar-item">
                    <router-link :to="{ name: 'add' }">Add Ticket</router-link>
                </div>
            </nav>
            <div class="columns">
                <div class="column">

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

    export default class App extends Vue {
        toggleClass = (e) => {
            let el = e.currentTarget.nextElementSibling;
            el.style.display = el.style.display === "none" ? "block" : "none";
        } 
    }
</script>

<style scoped>
</style>
