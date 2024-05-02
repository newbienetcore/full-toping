import { ActionReducerMap } from "@ngrx/store";
import { AnalyticsReducer, AnalyticsState } from "./analytics/analytics.reducer";
import { CRMReducer, CRMState } from "./crm/crm.reducer";
import { ECoReducer, ECoState } from "./ecommerce/ecommerce.reducer";
import { LearningReducer, LearningState } from "./learning/learning.reducer";
import { RealReducer, RealState } from "./real-estate/real-estate.reducer";
import { AppRealestateReducer, AppRealestateState } from "./app-realestate/apprealestate.reducer";
import { AgentReducer, AgentState } from "./agent/agent.reducer";
import { AgenciesReducer, AgenciesState } from "./agency/agency.reducer";
import { TicketReducer, TicketState } from "./tickets/ticket.reducer";
import { ChatReducer, ChatState } from "./chat/chat.reducer";
import { ProductReducer, ProductState } from "./product/product.reducer";
import { InvoiceReducer, InvoiceState } from "./invoices/invoices.reducer";
import { AuthenticationState, authenticationReducer } from "./authentication/authentication.reducer";
import { LayoutState, layoutReducer } from "./layouts/layout-reducers";
import { SelleReducer, SellerState } from "./seller/seller.reducer";
import { OrderReducer, OrderState } from "./orders/order.reducer";
import { InstructorReducer, InstructorState } from "./learning-instructor/instructor.reducer";
import { CustomerReducer, CustomerState } from "./customer/customer.reducer";
import { StudentsReducer, studentState } from "./students/student.reducer";
import { CourcesReducer, CourcesState } from "./learning-cources/cources.reducer";


export interface RootReducerState {
    layout: LayoutState,
    auth: AuthenticationState;
    statlist: AnalyticsState;
    CRMlist: CRMState;
    Ecommercelist: ECoState;
    Learninglist: LearningState;
    Realist: RealState;
    Apprealestate: AppRealestateState;
    Agentlist: AgentState;
    Agenciesdata: AgenciesState;
    ticketlist: TicketState;
    Chatmessage: ChatState;
    product: ProductState;
    Invoice: InvoiceState;
    Sellerlist: SellerState;
    Orderlist: OrderState;
    LearningList: InstructorState;
    CustomerList: CustomerState;
    SubscriptionList: studentState;
    CourcesList: CourcesState;
    Instructorlist: InstructorState
}

export const rootReducer: ActionReducerMap<RootReducerState> = {
    layout: layoutReducer,
    statlist: AnalyticsReducer,
    CRMlist: CRMReducer,
    auth: authenticationReducer,
    Ecommercelist: ECoReducer,
    Learninglist: LearningReducer,
    Realist: RealReducer,
    Apprealestate: AppRealestateReducer,
    Agentlist: AgentReducer,
    Agenciesdata: AgenciesReducer,
    ticketlist: TicketReducer,
    Chatmessage: ChatReducer,
    product: ProductReducer,
    Invoice: InvoiceReducer,
    Sellerlist: SelleReducer,
    Orderlist: OrderReducer,
    LearningList: InstructorReducer,
    CustomerList: CustomerReducer,
    SubscriptionList: StudentsReducer,
    CourcesList: CourcesReducer,
    Instructorlist: InstructorReducer
}