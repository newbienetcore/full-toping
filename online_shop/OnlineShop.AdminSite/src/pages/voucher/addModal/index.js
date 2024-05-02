import { Modal, Form, Select, Input } from 'antd'; 
import { useEffect, useState } from 'react';
import formatDate from './../../../utils/date.util';

const { Option } = Select;
const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 }
};

const UpdateModal = ({ isShow, onClose, onSave, voucher }) => {
  const [form] = Form.useForm();
  const [dataVoucher, setDataVoucher] = useState(voucher);

  useEffect(() => {
    setDataVoucher(voucher);
  }, [voucher]);

  const handleOk = () => {
    onSave();
  };

  const onFinish = (values) => {
    console.log(values);
  };

  const onGenderChange = () => {
    // switch (value) {
    //   case 'male':
    //     form.setFieldsValue({ note: 'Hi, man!' });
    //     break;
    //   case 'female':
    //     form.setFieldsValue({ note: 'Hi, lady!' });
    //     break;
    //   case 'other':
    //     form.setFieldsValue({ note: 'Hi there!' });
    //     break;
    //   default:
    // }
  };

  const formFields = [
    {
      name: ['code'],
      value: dataVoucher?.code
    },
    {
      name: ['discountAmount'],
      value: dataVoucher?.discountAmount
    },
    {
      name: ['discountPercent'],
      value: dataVoucher?.discountPercent
    },
    {
      name: ['project_language'],
      value: dataVoucher?.project_language
    },
    {
      name: ['startDate'],
      value: formatDate(dataVoucher?.startDate)
    },
    {
      name: ['endDate'],
      value: formatDate(dataVoucher?.endDate)
    },
    {
      name: ['status'],
      value: dataVoucher?.status
    }
  ];

  return (
    <Modal title="Create" open={isShow} onOk={handleOk} onCancel={onClose}>
      <Form {...layout} form={form} name="control-hooks" onFinish={onFinish} style={{ maxWidth: 600 }} fields={formFields}>
        <Form.Item name="code" label="Code">
          <Input placeholder="Option" />
        </Form.Item>
        <Form.Item name="discountAmount" label="DiscountAmount" rules={[{ required: true }]}>
          <Input placeholder="50 000 (VND)" />
        </Form.Item>
        <Form.Item name="discountPercent" label="DiscountPercent" rules={[{ required: true }]}>
          <Input placeholder="40%" />
        </Form.Item>
        <Form.Item name="startDate" label="StartDate" rules={[{ required: true }]}>
          <Input type="date" />
        </Form.Item>
        <Form.Item name="endDate" label="EndDate" rules={[{ required: true }]}>
          <Input type="date" />
        </Form.Item>
        <Form.Item name="status" label="Status" rules={[{ required: true }]}>
          <Select placeholder="Select status for voucher" onChange={onGenderChange} allowClear>
            <Option value="true">enable</Option>
            <Option value="false">disable</Option>
          </Select>
        </Form.Item>
        <Form.Item noStyle shouldUpdate={(prevValues, currentValues) => prevValues.gender !== currentValues.gender}>
          {({ getFieldValue }) =>
            getFieldValue('gender') === 'other' ? (
              <Form.Item name="customizeGender" label="Customize Gender" rules={[{ required: true }]}>
                <Input />
              </Form.Item>
            ) : null
          }
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default UpdateModal;
