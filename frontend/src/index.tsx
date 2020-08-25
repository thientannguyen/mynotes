import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

//css-tricks.com/the-trick-to-viewport-units-on-mobile/
function appHeight() {
    console.log('Run App Height..');
    let vh = window.innerHeight / 100;
    (document.querySelector(':root') as HTMLElement).style.setProperty(
        '--vh',
        `${vh}px`
    );
}
window.addEventListener('resize', appHeight);
appHeight();

ReactDOM.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>,
    document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
