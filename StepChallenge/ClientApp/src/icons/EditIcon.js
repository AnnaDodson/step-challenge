import React from 'react'

export const EditIcon = ({ color, size }) => (
    <svg role="img" xmlns="http://www.w3.org/2000/svg" width={ size } height={ size } viewBox="0 0 24 24" aria-labelledby="editIconTitle" stroke="#2329D6" strokeWidth="1" strokeLinecap="square" strokeLinejoin="miter" fill="none" color="#2329D6">
     <title id="editIconTitle">Edit</title>
     <path d="M18.4142136 4.41421356L19.5857864 5.58578644C20.366835 6.36683502 20.366835 7.63316498 19.5857864 8.41421356L8 20 4 20 4 16 15.5857864 4.41421356C16.366835 3.63316498 17.633165 3.63316498 18.4142136 4.41421356zM14 6L18 10"/>
    </svg>
  );

export const OkIcon = ({ color, size }) => (
  <svg role="img" xmlns="http://www.w3.org/2000/svg" width={size} height={size} viewBox="0 0 24 24" ariaLabelledby="okIconTitle" stroke="#2329D6" strokeWidth="1" strokeLinecap="square" strokeLinejoin="miter" fill="none" color="#2329D6">
     <title id="okIconTitle">Ok</title>
     <polyline points="4 13 9 18 20 7"/>
  </svg>
  );
   
  export const ICON = {
    COLORS: {
      BRAND: '#106fa5',
    },
    SIZES: {
      SML: 16,
      MED: 24,
    },
  };