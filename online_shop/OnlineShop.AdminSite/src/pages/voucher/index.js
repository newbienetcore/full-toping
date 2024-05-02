// material-ui
import { Grid, Typography } from '@mui/material';

// project import
import MainCard from 'components/MainCard';
import { Modal, Button, Space, Table } from 'antd';
import { useEffect, useState } from 'react';
import { Get as GetVoucher, Delete as DeleteVoucher } from 'services/voucher.service';
import UpdateModal from './addModal/index';

const VoucherPage = () => {
  const [vouchers, setVouchers] = useState([]);
  const [voucher, setVoucher] = useState({});

  const [isModelDelete, setIsModelDelete] = useState(false);
  const [isModalUpdate, setIsModalUpdate] = useState(false);

  const showModalUpdate = (data) => {
    setVoucher(data);
    setIsModalUpdate(true);
  };

  const onUpdate = () => {
    setIsModalUpdate(false);
  };

  const onClose = () => {
    setIsModalUpdate(false);
  };

  const showModalConfirmDelete = () => {
    setIsModelDelete(true);
  };

  const closeModalDelete = () => {
    setIsModelDelete(false);
  };

  // const [sortedInfo, setSortedInfo] = useState({});

  const handleChange = (pagination, filters, sorter) => {
    console.log('Various parameters', pagination, filters, sorter);
    setSortedInfo(sorter);
  };

  const columns = [
    {
      title: 'VoucherID',
      dataIndex: 'id',
      key: 'id',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'id' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'VoucherCode',
      dataIndex: 'code',
      key: 'code',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'code' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'Description',
      dataIndex: 'desc',
      key: 'desc',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'desc' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'DiscountAmount (VND)',
      dataIndex: 'discountAmount',
      key: 'discountAmount',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'discountAmount' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'DiscountPercent (%)',
      dataIndex: 'discountPercent',
      key: 'discountPercent',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'discountPercent' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'StartDate',
      dataIndex: 'startDate',
      key: 'startDate',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'startDate' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'EndDate',
      dataIndex: 'endDate',
      key: 'endDate',
      sorter: (a, b) => a.id - b.id
      // sortOrder: sortedInfo.columnKey === 'endDate' ? sortedInfo.order : null,
      // ellipsis: true
    },
    {
      title: 'Actions',
      key: 'action',
      render: (record) => (
        <Space size="middle">
          <Button type="primary" onClick={() => showModalUpdate(record)}>
            Update
          </Button>
          <Button
            onClick={() => {
              showModalConfirmDelete(record);
              setVoucher(record);
            }}
          >
            Delete
          </Button>
        </Space>
      )
    }
  ];

  const onDelete = () => {
    // eslint-disable-next-line no-debugger
    debugger;
    DeleteVoucher(voucher.code)
      .then(() => {
        fetchVoucher();
        closeModalDelete();
      })
      .catch((err) => {
        console.error('Failed to delete voucher', err);
      });
  };

  const fetchVoucher = () => {
    GetVoucher()
      .then((res) => {
        if (res) {
          setVouchers(res.items);
        }
      })
      .catch((err) => {
        console.log(err);
      });
  };

  useEffect(() => {
    fetchVoucher();
  }, []);

  return (
    <Grid container rowSpacing={4.5} columnSpacing={2.75}>
      <Grid item xs={12} md={12} lg={12}>
        <Grid container alignItems="center" justifyContent="space-between">
          <Grid item>
            <Typography variant="h5">Voucher list</Typography>
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
            dataSource={vouchers.map((voucher, index) => ({
              ...voucher,
              key: index
            }))}
            onChange={handleChange}
            pagination={{ defaultPageSize: vouchers.length, showSizeChanger: true }}
          />
        </MainCard>
        <UpdateModal isShow={isModalUpdate} onSave={onUpdate} onClose={onClose} voucher={voucher} />
        <Modal title="Confirm delete Modal" open={isModelDelete} onOk={onDelete} onCancel={closeModalDelete}>
          <p>Are you sure you want to delete this item ?</p>
        </Modal>
      </Grid>
    </Grid>
  );
};

export default VoucherPage;
