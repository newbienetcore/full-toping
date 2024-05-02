// material-ui
import { Grid, Typography } from '@mui/material';

// project import
import MainCard from 'components/MainCard';
import { Modal, Button, Space, Table } from 'antd';
import { useState } from 'react';
const data = [
  {
    key: '1',
    name: 'John Brown',
    age: 32,
    address: 'New York No. 1 Lake Park'
  },
  {
    key: '2',
    name: 'Jim Green',
    age: 42,
    address: 'London No. 1 Lake Park'
  },
  {
    key: '3',
    name: 'Joe Black',
    age: 32,
    address: 'Sydney No. 1 Lake Park'
  },
  {
    key: '4',
    name: 'Jim Red',
    age: 32,
    address: 'London No. 2 Lake Park'
  }
];
const RolePage = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [sortedInfo, setSortedInfo] = useState({});

  const handleChange = (pagination, filters, sorter) => {
    console.log('Various parameters', pagination, filters, sorter);
    setSortedInfo(sorter);
  };

  const handleOk = () => {
    setIsModalOpen(false);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.length - b.name.length,
      sortOrder: sortedInfo.columnKey === 'name' ? sortedInfo.order : null,
      ellipsis: true
    },
    {
      title: 'Age',
      dataIndex: 'age',
      key: 'age',
      sorter: (a, b) => a.age - b.age,
      sortOrder: sortedInfo.columnKey === 'age' ? sortedInfo.order : null,
      ellipsis: true
    },
    {
      title: 'Address',
      dataIndex: 'address',
      key: 'address',
      sorter: (a, b) => a.address.length - b.address.length,
      sortOrder: sortedInfo.columnKey === 'address' ? sortedInfo.order : null,
      ellipsis: true
    },
    {
      title: 'Actions',
      key: 'action',
      render: () => (
        <Space size="middle">
          <a>update</a>
          <a>Delete</a>
        </Space>
      )
    }
  ];
  const showModal = () => {
    setIsModalOpen(true);
  };

  return (
    <Grid container rowSpacing={4.5} columnSpacing={2.75}>
      <Grid item xs={12} md={12} lg={12}>
        <Grid container alignItems="center" justifyContent="space-between">
          <Grid item>
            <Typography variant="h5">Role list</Typography>
          </Grid>
          <Grid item />
        </Grid>
        <Space style={{ marginTop: 10 }}>
          <Button onClick={showModal}>Create</Button>
        </Space>
        <MainCard sx={{ mt: 2 }} content={false}>
          <Table columns={columns} dataSource={data} onChange={handleChange} />
        </MainCard>
        <Modal title="Basic Modal" open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
          <p>Some contents...</p>
          <p>Some contents...</p>
          <p>Some contents...</p>
        </Modal>
      </Grid>
    </Grid>
  );
};

export default RolePage;
