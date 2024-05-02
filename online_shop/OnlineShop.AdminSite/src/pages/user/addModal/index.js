import { Modal, Form, Select, Input } from 'antd';

const { Option } = Select;
const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 }
};

const UpdateModal = ({ isShow, onClose, onSave }) => {
  const [form] = Form.useForm();

  const handleOk = () => {
    onSave();
  };

  const onFinish = (values) => {
    console.log(values);
  };

  const onGenderChange = (value) => {
    switch (value) {
      case 'male':
        form.setFieldsValue({ note: 'Hi, man!' });
        break;
      case 'female':
        form.setFieldsValue({ note: 'Hi, lady!' });
        break;
      case 'other':
        form.setFieldsValue({ note: 'Hi there!' });
        break;
      default:
    }
  };

  return (
    <Modal title="Create" open={isShow} onOk={handleOk} onCancel={onClose}>
      <Form {...layout} form={form} name="control-hooks" onFinish={onFinish} style={{ maxWidth: 600 }}>
        <Form.Item name="note" label="Note" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="gender" label="Gender" rules={[{ required: true }]}>
          <Select placeholder="Select a option and change input text above" onChange={onGenderChange} allowClear>
            <Option value="male">male</Option>
            <Option value="female">female</Option>
            <Option value="other">other</Option>
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
