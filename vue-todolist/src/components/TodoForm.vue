<template>
    <el-form @submit.prevent :inline="true" :model="formInput">
        <el-form-item label="Title" prop="title">
            <el-input v-model="formInput.title" placeholder="Enter To Do" label-position="left"></el-input>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" @click="onSubmit()">Add</el-button>
        </el-form-item>
    </el-form>
    
</template>

<script lang="ts">
import { ElMessage } from 'element-plus';
import { Options, Vue } from 'vue-class-component';

@Options({})
export default class TodoForm extends Vue {
  formInput = { title: "", complete: false };

  onSubmit(){
    if(this.formInput.title.length > 3){
        this.$emit('send-message', this.formInput);
    } else {
        ElMessage({
            message: "Warning, this todo is too short!",
            type: "warning"     
        })
        this.formInput.title = "";
    }
    
  }
}
</script>

<style scoped>
/* Override the margin-right for inline form items */
.el-form--inline .el-form-item {
    margin-right: 8px; /* Adjust to your desired margin */
}
</style>