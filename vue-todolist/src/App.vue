<template>
  <el-container>
    <el-main>
      <TodoList></TodoList>
    </el-main>
  </el-container>
</template>

<script lang="ts">
import { Vue, Options } from 'vue-class-component';
import TodoList from './components/TodoList.vue';

@Options({
  components: {
    TodoList,
  },
})
export default class App extends Vue {}
</script>

<style>
body {
  margin: 0;
}

.el-container{
  background: #e9eef3;
  color: var(--el-text-color-primary);
  text-align: center;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
</style>

<!-- <script lang="ts">
import { defineComponent } from 'vue';
import HelloWorld from './components/HelloWorld.vue';
import TodoList from './components/TodoList.vue';

export default defineComponent({
  components: {
    HelloWorld,
    TodoList,
  },
});
</script> -->