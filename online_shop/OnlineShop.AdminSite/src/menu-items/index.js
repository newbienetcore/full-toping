// ==============================|| MENU ITEMS ||============================== //
// assets
import { UserOutlined } from '@ant-design/icons';

// icons
const icons = {
  UserOutlined
};

const menuItems = {
  items: [
    {
      id: 'group-user',
      title: 'Navigation',
      type: 'group',
      children: [
        {
          id: 'user',
          title: 'User Page',
          type: 'item',
          url: '/users',
          icon: icons.UserOutlined,
          breadcrumbs: false
        },
        {
          id: 'customer',
          title: 'Customer Page',
          type: 'item',
          url: '/customers',
          icon: icons.UserOutlined,
          breadcrumbs: false
        },
        {
          id: 'role',
          title: 'Role Page',
          type: 'item',
          url: '/roles',
          icon: icons.UserOutlined,
          breadcrumbs: false
        },
        {
          id: 'voucher',
          title: 'Voucher Page',
          type: 'item',
          url: '/vouchers',
          icon: icons.UserOutlined,
          breadcrumbs: false
        }
      ]
    }
  ]
};

export default menuItems;
