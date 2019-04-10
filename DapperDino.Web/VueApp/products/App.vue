<template>
    <div>
        <div class="container">
            <div class="row">
                <div class="col-xs-12">
                   
                    <Products />

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
