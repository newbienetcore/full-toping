// material-ui
import { Grid, Typography } from '@mui/material';

// project import
import MainCard from 'components/MainCard';
import { Modal, Button, Space, Table } from 'antd';
import { useEffect, useState } from 'react';
import { Get as GetUser } from 'services/user.service';
import UpdateModal from '../voucher/addModal/index';

const UserPage = () => {
  const [users, setUsers] = useState([]);

  const [isModelDelete, setIsModelDelete] = useState(false);
  const [isModalUpdate, setIsModalUpdate] = useState(false);

  const showModalUpdate = () => {
    setIsModalUpdate(true);
  };

  const onUpdate = (data) => {
    console.log(data);
    setIsModalUpdate(false);
  };

  const onClose = () => {
    setIsModalUpdate(false);
  };

  const showModalConfirmDelete = (item) => {
    console.log(item);
    setIsModelDelete(true);
  };

  const closeModalDelete = () => {
    setIsModelDelete(false);
  };

  const [sortedInfo, setSortedInfo] = useState({});

  const handleChange = (pagination, filters, sorter) => {
    console.log('Various parameters', pagination, filters, sorter);
    setSortedInfo(sorter);
  };

  const columns = [
    {
      title: 'UserName',
      dataIndex: 'userName',
      key: 'userName',
      sorter: (a, b) => a.userName.length - b.userName.length,
      sortOrder: sortedInfo.columnKey === 'userName' ? sortedInfo.order : null,
      ellipsis: true
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      sorter: (a, b) => a.email - b.email,
      sortOrder: sortedInfo.columnKey === 'email' ? sortedInfo.order : null,
      ellipsis: true
    },
    {
      title: 'Actions',
      key: 'action',
      render: (record) => (
        <Space size="middle">
          <Button type="primary" onClick={() => showModalUpdate(record)}>
            Update
          </Button>
          <Button onClick={() => showModalConfirmDelete(record)}>Delete</Button>
        </Space>
      )
    }
  ];

  const onDelete = (item) => {
    console.log(item);
  };

  const fetchUser = () => {
    GetUser()
      .then((res) => {
        if (res) {
          setUsers(res.items);
        }
      })
      .catch((err) => {
        console.log(err);
      });
  };

  useEffect(() => {
    fetchUser();
  }, []);

  return (
    <Grid container rowSpacing={4.5} columnSpacing={2.75}>
      <Grid item xs={12} md={12} lg={12}>
        <Grid container alignItems="center" justifyContent="space-between">
          <Grid item>
            <Typography variant="h5">User list</Typography>
          </Grid>
          <Grid item />
        </Grid>
        <Space style={{ marginTop: 10 }}>
          <Button type="primary" onClick={showModalUpdate}>
            Create
          </Button>
        </Space>
        <MainCard sx={{ mt: 2 }} content={false}>
          <Table
            columns={columns}
            dataSource={users.map((user, index) => ({
              ...user,
              key: index
            }))}
            onChange={handleChange}
          />
        </MainCard>
        <UpdateModal isShow={isModalUpdate} onSave={onUpdate} onClose={onClose} />
        <Modal title="Confirm delete Modal" open={isModelDelete} onOk={onDelete} onCancel={closeModalDelete}>
          <p>Are you sure you want to delete this item ?</p>
        </Modal>
      </Grid>
    </Grid>
  );
};

export default UserPage;
