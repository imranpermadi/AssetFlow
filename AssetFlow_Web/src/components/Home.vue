<template>
  <div>

  </div>
</template>

<script>
    export default {
        name: 'Home',
        props: {
            msg: String
        },
        data(){
            return{
                sapOrderTotal: 0,
                sapOrderWaitingApproval: 0,
                sapOrderApproved: 0,

                orderTotal: 0,
                orderWaitingApproval: 0,
                orderCancelled: 0,
                orderRejected: 0,
                orderAssigned: 0,
                orderCompleted: 0,

                shipmentTotal: 0,
                shipmentNew: 0,
                shipmentAppointment: 0,
                shipmentRegisIn: 0,
                shipmentRegisOut: 0,
                shipmentCompleted: 0,

                completedData: null,
                dataUrl: "dashboard/",

                // basicData: {
                //     labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                //     datasets: [
                //         {
                //             label: 'My First dataset',
                //             backgroundColor: '#42A5F5',
                //             data: [65, 59, 80, 81, 56, 55, 40]
                //         },
                //         {
                //             label: 'My Second dataset',
                //             backgroundColor: '#FFA726',
                //             data: [28, 48, 40, 19, 86, 27, 90]
                //         }
                //     ]
                // },
                basicOptions: {
                    plugins: {
                        legend: {
                            labels: {
                                color: '#495057'
                            }
                        }
                    },
                    scales: {
                        x: {
                            ticks: {
                                color: '#495057'
                            },
                            grid: {
                                color: '#ebedef'
                            }
                        },
                        y: {
                            ticks: {
                                color: '#495057'
                            },
                            grid: {
                                color: '#ebedef'
                            }
                        }
                    }
                },
            }
        },
        methods:{
            showToast(){
                this.$toast.add({ severity: 'error', summary: "Error", detail: "data", life: 120000});
            },
            getDataCompletedChart(){
                var self = this;

                this.$axios.get(this.$API_URL + this.dataUrl + "completed")
                    .then((response) => {
                        if(response && response.data.success){
                            self.completedData = response.data.data;
                        }
                    })
                    .catch(function (err) {
                        console.log(err);
                    });
            },
            getStatusCount(){
                var self = this;

                this.$axios.get(this.$API_URL + this.dataUrl + "status_count")
                    .then((response) => {
                        if(response && response.data.success){
                            var d = response.data.data;
                            self.sapOrderTotal = d.sap_order_total;
                            self.sapOrderWaitingApproval = d.sap_order_waiting_approval;
                            self.sapOrderApproved = d.sap_order_approved;
                            self.orderTotal = d.order_total;
                            self.orderWaitingApproval = d.order_waiting_approval;
                            self.orderAssigned = d.order_assigned;
                            self.orderCancelled = d.order_cancelled;
                            self.orderRejected = d.order_rejected;
                            self.orderCompleted = d.order_completed;
                            self.shipmentTotal = d.shipment_total;
                            self.shipmentNew = d.shipment_new;
                            self.shipmentAppointment = d.shipment_appointment;
                            self.shipmentRegisIn = d.shipment_regis_in;
                            self.shipmentRegisOut = d.shipment_regis_out;
                            self.shipmentCompleted = d.shipment_completed;

                        }
                    })
                    .catch(function(err){
                        console.log(err);
                    });
            },
            test(){
                var self = this;

                this.$axios.get(this.$API_URL + "dashboard/test")
                    .then((response) => {

                    })
                    .catch(function(err){
                        console.log(err);
                    });
            }
        },
        mounted(){
            if(!this.$store.getters.isAuthenticated){
                this.$router.push({ name: "Login", params: { sessionExpired: true} });
            }


            // this.test();

            
            // this.getDataCompletedChart();
            // this.getStatusCount();
        },
        computed:{
            // showDashboard(){
            //     return this.$store.getters.getIsUser;
            // },
            // showOrder(){
            //     return this.$store.getters.getIsAdmin || (this.$store.getters.getIsUser && this.$store.getters.getDepartment != this.$DEPARTMENTS.WAREHOUSE);
            // },
            // showSAPOrder(){
            //     return this.$store.getters.getIsAdmin || (this.$store.getters.getIsUser && this.$store.getters.getDepartment != this.$DEPARTMENTS.WAREHOUSE);
            // },
            // showShipment(){
            //     return this.$store.getters.getIsUser;
            // }
        }
    }

    //  var values = [],
    //     keys = Object.keys(localStorage),
    //     i = keys.length;

    // while ( i-- ) {
    //     values.push( localStorage.getItem(keys[i]) );
    // }

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    h3 {
        margin: 40px 0 0;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        display: inline-block;
        margin: 0 10px;
    }

    a {
        color: #42b983;
    }
</style>
