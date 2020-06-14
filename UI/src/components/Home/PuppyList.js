import PuppyPreview from './PuppyPreview';
import ListPagination from '../ListPagination';
import React from 'react';

const PuppyList = props => {
  if (!props.animals) {
    return (
      <div className="article-preview">Czekam...</div>
    );
  }

  if (props.animals.length === 0) {
    return (
      <div className="article-preview">
        No puppies are here... yet.
      </div>
    );
  }

  return (
    <div className="card-deck">
      {
        props.animals.map(article => {
          return (
            <PuppyPreview article={article} key={article.slug} />
          );
        })
      }

      <ListPagination
        pager={props.pager}
        articlesCount={props.articlesCount}
        currentPage={props.currentPage} />
    </div>
  );
};

export default PuppyList;
