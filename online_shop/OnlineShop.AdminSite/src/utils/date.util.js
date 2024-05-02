const formatDate = (dateString) => {
  if (!dateString) return ''; // Handle cases where dateString is null or undefined
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0'); // Add leading zero if needed
  const day = String(date.getDate()).padStart(2, '0'); // Add leading zero if needed
  return `${year}-${month}-${day}`;
};

export default formatDate;