import { FaEdit, FaTrash, FaCheckCircle, FaClock, FaHourglassHalf } from 'react-icons/fa';

const TaskItem = ({ task, onEdit, onDelete }) => {
  const getStatusColor = (status) => {
    const colors = {
      'Pending': 'bg-yellow-100 text-yellow-800 border-yellow-300',
      'InProgress': 'bg-blue-100 text-blue-800 border-blue-300',
      'Completed': 'bg-green-100 text-green-800 border-green-300'
    };
    return colors[status] || 'bg-gray-100 text-gray-800 border-gray-300';
  };

  const getStatusIcon = (status) => {
    const icons = {
      'Pending': <FaClock className="inline mr-1" />,
      'InProgress': <FaHourglassHalf className="inline mr-1" />,
      'Completed': <FaCheckCircle className="inline mr-1" />
    };
    return icons[status] || null;
  };

  const getStatusText = (status) => {
    const texts = {
      'Pending': 'Pendente',
      'InProgress': 'Em Progresso',
      'Completed': 'Concluída'
    };
    return texts[status] || status;
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow border border-gray-200">
      <div className="flex justify-between items-start mb-3">
        <h3 className="text-xl font-bold text-gray-800 flex-1">{task.title}</h3>
        <div className="flex gap-2 ml-4">
          <button
            onClick={() => onEdit(task)}
            className="text-blue-600 hover:text-blue-800 transition p-2 hover:bg-blue-50 rounded"
            title="Editar"
          >
            <FaEdit size={18} />
          </button>
          <button
            onClick={() => onDelete(task.id)}
            className="text-red-600 hover:text-red-800 transition p-2 hover:bg-red-50 rounded"
            title="Excluir"
          >
            <FaTrash size={18} />
          </button>
        </div>
      </div>

      <p className="text-gray-600 mb-4">{task.description}</p>

      <div className="flex flex-wrap gap-2 items-center text-sm">
        <span className={`px-3 py-1 rounded-full border ${getStatusColor(task.status)}`}>
          {getStatusIcon(task.status)}
          {getStatusText(task.status)}
        </span>
        <span className="text-gray-500">
          📅 Criada em: {formatDate(task.createdAt)}
        </span>
        {task.updatedAt && (
          <span className="text-gray-500">
            ✏️ Atualizada em: {formatDate(task.updatedAt)}
          </span>
        )}
      </div>
    </div>
  );
};

export default TaskItem;