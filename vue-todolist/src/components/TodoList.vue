<template>
    <el-row>
        <el-col :span="12" :offset="7" style="width: 100%">
            <h1>TodoList</h1>
            <TodoForm @send-message="createTodo"></TodoForm>
            <el-table :data="todos">
                <el-table-column prop="title" label="Title" width="350"></el-table-column>
                <el-table-column fixed="right" label="Operation" width="200">
                    <template #default="scope">
                        <el-space wrap>
                            <el-switch 
                                v-model="scope.row.complete" 
                                @click="updateTodo(scope.row)"
                            />
                            <el-popconfirm 
                                comfirm-button-text="Yes" 
                                cancel-button-text="No" 
                                icon="el-icon-info" 
                                icon-color="red" 
                                title="Are sure you want to delete this?" 
                                @comfirm="deleteTodo(scope.row)" 
                                @cancel="cancelDelete()">
                                <template #reference>
                                    <el-button 
                                        size="small" 
                                        type="danger">
                                        Delete
                                    </el-button>
                                </template>
                            </el-popconfirm>
                        </el-space>
                    </template>
                </el-table-column>
            </el-table>
        </el-col>
    </el-row>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component';
import TodoForm from './TodoForm.vue';
import { ElMessage } from 'element-plus';

interface Todo{
    id: number;
    title: string;
    complete: boolean;
}

@Options({
    components: {
        TodoForm,
    },
})

export default class TodoList extends Vue {
    todos: Todo[] = [];

    async mounted(){
        await this.loadTodos();
    }

    async loadTodos(){
        try {
            const response = await this.axios.get('https://localhost:7230/api/todos');
            this.todos = response.data;
        } catch (error) {
            console.error("Error loading todos:", error);
            ElMessage({
                message: "Failed to load todos",
                type: "error"
            });
        }
    }

    async createTodo(todo: Todo) {
        console.log("Todo", todo);

        try {
            await this.axios.post('https://localhost:7230/api/todos', {
                title: todo.title,
                complete: todo.complete
            });
            ElMessage({
                message: "Todo Created",
                type: "success"
            });
            await this.loadTodos();
        } catch (error) {
            console.error("Error creating todo:", error);
            ElMessage({
                message: "Failed to create todo",
                type: "error"
            });
        }
    }

    async updateTodo(todo: Todo){
        console.log("Todo", todo)

        try {
            await this.axios.put(`https://localhost:7230/api/todos/${todo.id}`, {
                title: todo.title,
                complete: todo.complete
            });
            ElMessage({
                message: "Todo Updated",
                type: "success"
            });
            await this.loadTodos();
        } catch (error) {
            console.error("Error updating todo:", error);
            ElMessage({
                message: "Failed to update todo",
                type: "error"
            });
        }
    }

    async deleteTodo(todo: Todo){
        // await this.axios.delete(`https://localhost:7230/api/todos/${todo.id}`);
        try {
            await this.axios.delete(`https://localhost:7230/api/todos/${todo.id}`);
            ElMessage({
                message: "Todo Deleted",
                type: "success"
            });
            await this.loadTodos();
        } catch (error) {
            console.error("Error deleting todo:", error);
            ElMessage({
                message: "Failed to delete todo",
                type: "error"
            });
        }
    }

    cancelDelete(){
        console.log("Canceled the Delete");
    }
  
}
</script>