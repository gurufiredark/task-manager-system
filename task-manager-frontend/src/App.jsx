import { useState, useEffect } from 'react';
import { FaPlus, FaSpinner } from 'react-icons/fa';
import { taskService } from './services/api';
import TaskForm from './components/TaskForm';
import TaskItem from './components/TaskItem';
import FilterBar from './components/FilterBar';

function App() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [editingTask, setEditingTask] = useState(null);
  const [filters, setFilters] = useState({});

  const loadTasks = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await taskService.getAllTasks(filters);
      setTasks(data);
    } catch (err) {
      setError('Erro ao carregar tarefas. Verifique se a API está rodando.');
      console.error('Erro ao carregar tarefas:', err);
    } finally {
      setLoading(false);
    }
  };

  // Carrega tarefas ao montar o componente e quando filtros mudam
  useEffect(() => {
    loadTasks();
  }, [filters]);

  const handleCreateTask = async (taskData) => {
    try {
      await taskService.createTask(taskData);
      setShowForm(false);
      loadTasks();
    } catch (err) {
      alert('Erro ao criar tarefa: ' + err.message);
      console.error('Erro ao criar tarefa:', err);
    }
  };

  const handleUpdateTask = async (taskData) => {
    try {
      await taskService.updateTask(editingTask.id, taskData);
      setEditingTask(null);
      setShowForm(false);
      loadTasks();
    } catch (err) {
      alert('Erro ao atualizar tarefa: ' + err.message);
      console.error('Erro ao atualizar tarefa:', err);
    }
  };

  const handleDeleteTask = async (id) => {
    if (!window.confirm('Tem certeza que deseja excluir esta tarefa?')) {
      return;
    }

    try {
      await taskService.deleteTask(id);
      loadTasks();
    } catch (err) {
      alert('Erro ao deletar tarefa: ' + err.message);
      console.error('Erro ao deletar tarefa:', err);
    }
  };

  const handleEditTask = (task) => {
    setEditingTask(task);
    setShowForm(true);
  };

  const handleCloseForm = () => {
    setShowForm(false);
    setEditingTask(null);
  };

  const handleFilterChange = (newFilters) => {
    setFilters(newFilters);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <div className="container mx-auto px-4 py-8 max-w-7xl">
        {/* Header */}
        <div className="bg-white rounded-lg shadow-lg p-6 mb-6">
          <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
            <div>
              <h1 className="text-4xl font-bold text-gray-800 mb-2">
                📋 Gerenciador de Tarefas
              </h1>
              <p className="text-gray-600">
                Organize suas tarefas de forma simples e eficiente
              </p>
            </div>
            <button
              onClick={() => setShowForm(true)}
              className="flex items-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition shadow-md hover:shadow-lg"
            >
              <FaPlus />
              <span className="font-semibold">Nova Tarefa</span>
            </button>
          </div>
        </div>

        {/* Filtros */}
        <FilterBar onFilterChange={handleFilterChange} />

        {/* Loading */}
        {loading && (
          <div className="flex justify-center items-center py-20">
            <FaSpinner className="animate-spin text-blue-600 text-5xl" />
          </div>
        )}

        {/* Caso de Erro */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-6 py-4 rounded-lg mb-6">
            <p className="font-semibold">⚠️ {error}</p>
            <p className="text-sm mt-2">
              Certifique-se de que a API está rodando em http://localhost:5239
            </p>
          </div>
        )}

        {/* Lista das Tarefas */}
        {!loading && !error && (
          <>
            {tasks.length === 0 ? (
              <div className="bg-white rounded-lg shadow-md p-12 text-center">
                <div className="text-6xl mb-4">📝</div>
                <h3 className="text-2xl font-semibold text-gray-700 mb-2">
                  Nenhuma tarefa encontrada
                </h3>
                <p className="text-gray-500 mb-6">
                  Comece criando a primeira tarefa
                </p>
                <button
                  onClick={() => setShowForm(true)}
                  className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
                >
                  <FaPlus className="inline mr-2" />
                  Criar Primeira Tarefa
                </button>
              </div>
            ) : (
              <>
                <div className="mb-4 text-gray-600">
                  <span className="font-semibold">{tasks.length}</span> tarefa(s) encontrada(s)
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                  {tasks.map((task) => (
                    <TaskItem
                      key={task.id}
                      task={task}
                      onEdit={handleEditTask}
                      onDelete={handleDeleteTask}
                    />
                  ))}
                </div>
              </>
            )}
          </>
        )}

        {/* Modal de Formulário */}
        {showForm && (
          <TaskForm
            onSubmit={editingTask ? handleUpdateTask : handleCreateTask}
            onClose={handleCloseForm}
            initialData={editingTask}
          />
        )}
      </div>

      <footer className="text-center py-6 text-gray-600">
        <p>Desenvolvido por Gabriel</p>
      </footer>
    </div>
  );
}

export default App;