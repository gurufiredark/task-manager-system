import { useState } from 'react';
import { FaFilter, FaSort } from 'react-icons/fa';

const FilterBar = ({ onFilterChange }) => {
  const [filters, setFilters] = useState({
    status: '',
    orderBy: 'createdAt',
    orderDirection: 'desc',
    createdAfter: '',
    createdBefore: ''
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    const newFilters = {
      ...filters,
      [name]: value
    };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleClearFilters = () => {
    const clearedFilters = {
      status: '',
      orderBy: 'createdAt',
      orderDirection: 'desc',
      createdAfter: '',
      createdBefore: ''
    };
    setFilters(clearedFilters);
    onFilterChange(clearedFilters);
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-6 mb-6">
      <div className="flex items-center gap-2 mb-4">
        <FaFilter className="text-blue-600" />
        <h3 className="text-lg font-semibold text-gray-800">Filtros e Ordenação</h3>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
        {/* Status */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Status
          </label>
          <select
            name="status"
            value={filters.status}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="">Todos</option>
            <option value="0">Pendente</option>
            <option value="1">Em Progresso</option>
            <option value="2">Concluída</option>
          </select>
        </div>

        {/* Data Inicial */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Criada Após
          </label>
          <input
            type="date"
            name="createdAfter"
            value={filters.createdAfter}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>

        {/* Data Final */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Criada Antes
          </label>
          <input
            type="date"
            name="createdBefore"
            value={filters.createdBefore}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>

        {/* Ordenar Por */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            <FaSort className="inline mr-1" />
            Ordenar Por
          </label>
          <select
            name="orderBy"
            value={filters.orderBy}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="createdAt">Data de Criação</option>
            <option value="title">Título</option>
            <option value="status">Status</option>
          </select>
        </div>

        {/* Direção */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Direção
          </label>
          <select
            name="orderDirection"
            value={filters.orderDirection}
            onChange={handleChange}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="asc">Crescente</option>
            <option value="desc">Decrescente</option>
          </select>
        </div>
      </div>
      <div className="mt-4">
        <button
          onClick={handleClearFilters}
          className="px-4 py-2 text-sm text-gray-600 hover:text-gray-800 hover:bg-gray-100 rounded-lg transition"
        >
          Limpar Filtros
        </button>
      </div>
    </div>
  );
};

export default FilterBar;