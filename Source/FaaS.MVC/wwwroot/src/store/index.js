import {createStore} from "redux";
import reducers from "../reducers";
import middleware from "../middleware";

const store = createStore(reducers, middleware);

store.dispatch((dispatch) => {

});

export default store;